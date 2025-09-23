namespace PortalVR.Gadgets.Laser
{
    public interface IHittable
    {
        public void LaserEnter(LaserEmitter emitter);
        public void LaserExit(LaserEmitter emitter);
    }
}
