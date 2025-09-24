using System;

namespace PortalVR.Gadgets.Laser
{
    public class Trigger : HittableBase
    {
        private void OnEnable() => _laserManager?.Register(this);

        private void OnDisable() => _laserManager?.Unregister(this);
    }
}
