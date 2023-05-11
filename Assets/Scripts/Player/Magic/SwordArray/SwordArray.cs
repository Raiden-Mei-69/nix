namespace Player.Magic
{
    public class SwordArray : MagicBase
    {

        private int count = 0;
        public SwordArraySword[] swordArraySwords;
        public float[] RandomDelayLaunch = new float[2] { 1f, 3f };
        public override void LaunchMagic()
        {
            base.LaunchMagic();
            swordArraySwords = GetComponentsInChildren<SwordArraySword>();
            foreach (var sword in swordArraySwords)
            {
                sword.OnCreate(MagicDamage, target, ProjSpeed, rotationSpeed, RandomDelayLaunch, this, player);
            }
        }

        public void OnDestroyChild()
        {
            count++;
            if (count >= swordArraySwords.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}