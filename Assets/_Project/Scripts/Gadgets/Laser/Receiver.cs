namespace PortalVR.Gadgets.Laser
{
    public class Receiver : HittableBase
    {
        private void OnEnable() => _laserManager?.Register(this);

        private void OnDisable() => _laserManager?.Unregister(this);
    }
}
