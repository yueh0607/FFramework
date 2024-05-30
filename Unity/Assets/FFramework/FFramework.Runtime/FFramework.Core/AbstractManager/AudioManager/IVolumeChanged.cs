namespace FFramework
{
    public interface IVolumeChanged  : ISendEvent<float ,float>
    {
        void OnVolumeChanged(float oldVolume, float newVolume);
    }
}
