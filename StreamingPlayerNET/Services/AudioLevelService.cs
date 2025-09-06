using NAudio.Wave;
using NLog;
using System.Diagnostics;

namespace StreamingPlayerNET.Services;

public class AudioLevelService : IDisposable
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    private SampleAggregator? _sampleAggregator;
    private IWaveProvider? _currentProvider;
    private System.Windows.Forms.Timer? _levelTimer;
    private bool _isMonitoring = false;
    private WaveOutEvent? _waveOut;
    
    public event EventHandler<float>? AudioLevelChanged;
    
    public AudioLevelService()
    {
        Logger.Debug("Audio Level Service initialized");
        SetupLevelTimer();
    }
    
    private void SetupLevelTimer()
    {
        _levelTimer = new System.Windows.Forms.Timer();
        _levelTimer.Interval = 50; // Update every 50ms for responsive VU meter
        _levelTimer.Tick += LevelTimer_Tick;
    }
    
    private void LevelTimer_Tick(object? sender, EventArgs e)
    {
        if (_sampleAggregator != null && _isMonitoring)
        {
            var rms = _sampleAggregator.RootMeanSquare;
            
            // Convert to dB and normalize to 0-100 range
            var level = Math.Max(0, Math.Min(100, (float)(20 * Math.Log10(Math.Max(rms, 0.0001)) + 60)));
            
            AudioLevelChanged?.Invoke(this, level);
        }
    }
    
    public void StartMonitoring(IWaveProvider waveProvider, WaveOutEvent waveOut)
    {
        if (waveProvider == null)
        {
            Logger.Warn("Cannot start audio level monitoring: waveProvider is null");
            return;
        }
        
        if (waveOut == null)
        {
            Logger.Warn("Cannot start audio level monitoring: waveOut is null");
            return;
        }
        
        if (_isMonitoring)
        {
            StopMonitoring();
        }
        
        _currentProvider = waveProvider;
        _waveOut = waveOut;
        _sampleAggregator = new SampleAggregator();
        
        // Create a custom wave provider that aggregates samples
        var aggregatingProvider = new AggregatingWaveProvider(_currentProvider, _sampleAggregator);
        
        // Replace the wave provider in the wave out
        _waveOut.Init(aggregatingProvider);
        
        _isMonitoring = true;
        _levelTimer?.Start();
        
        Logger.Debug("Started audio level monitoring");
    }
    
    public void StopMonitoring()
    {
        _isMonitoring = false;
        _levelTimer?.Stop();
        _sampleAggregator?.Reset();
        
        Logger.Debug("Stopped audio level monitoring");
    }
    
    public void Dispose()
    {
        StopMonitoring();
        _levelTimer?.Dispose();
    }
}

// Sample aggregator for calculating RMS and peak values
public class SampleAggregator
{
    private readonly object _lockObject = new object();
    private float _rms;
    private float _peak;
    private int _sampleCount;
    
    public float RootMeanSquare
    {
        get
        {
            lock (_lockObject)
            {
                return _sampleCount > 0 ? (float)Math.Sqrt(_rms / _sampleCount) : 0;
            }
        }
    }
    
    public float PeakValue
    {
        get
        {
            lock (_lockObject)
            {
                return _peak;
            }
        }
    }
    
    public void Add(float value)
    {
        lock (_lockObject)
        {
            _rms += value * value;
            _peak = Math.Max(_peak, Math.Abs(value));
            _sampleCount++;
            
            // Reset periodically to prevent overflow
            if (_sampleCount > 1000)
            {
                _rms = _rms / _sampleCount;
                _sampleCount = 1;
            }
        }
    }
    
    public void Reset()
    {
        lock (_lockObject)
        {
            _rms = 0;
            _peak = 0;
            _sampleCount = 0;
        }
    }
}

// Custom wave provider that aggregates samples
public class AggregatingWaveProvider : IWaveProvider
{
    private readonly IWaveProvider _sourceProvider;
    private readonly SampleAggregator _aggregator;
    
    public AggregatingWaveProvider(IWaveProvider sourceProvider, SampleAggregator aggregator)
    {
        _sourceProvider = sourceProvider ?? throw new ArgumentNullException(nameof(sourceProvider));
        _aggregator = aggregator ?? throw new ArgumentNullException(nameof(aggregator));
    }
    
    public WaveFormat WaveFormat => _sourceProvider.WaveFormat;
    
    public int Read(byte[] buffer, int offset, int count)
    {
        var bytesRead = _sourceProvider.Read(buffer, offset, count);
        
        if (bytesRead > 0 && WaveFormat.BitsPerSample == 16)
        {
            // Convert bytes to samples and aggregate
            var samples = bytesRead / 2; // 16-bit = 2 bytes per sample
            for (int i = 0; i < samples; i++)
            {
                var sampleIndex = offset + i * 2;
                if (sampleIndex + 1 < buffer.Length)
                {
                    var sample = BitConverter.ToInt16(buffer, sampleIndex);
                    var normalizedSample = sample / 32768f; // Normalize to -1 to 1
                    _aggregator.Add(normalizedSample);
                }
            }
        }
        
        return bytesRead;
    }
} 