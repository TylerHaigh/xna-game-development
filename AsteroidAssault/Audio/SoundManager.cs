using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidAssault.Audio
{
    static class SoundManager
    {
        private static List<SoundEffect> Explosions = new List<SoundEffect>();
        private static int ExplosionCount = 4;

        private static SoundEffect PlayerShot;
        private static SoundEffect EnemyShot;

        private static Random Rand = new Random();

        public static void Initialise(ContentManager content)
        {
            try
            {
                PlayerShot = content.Load<SoundEffect>(@"Sounds\Shot1");
                EnemyShot = content.Load<SoundEffect>(@"Sounds\Shot2");

                for (int i = 1; i <= ExplosionCount; i++)
                {
                    SoundEffect se = content.Load<SoundEffect>($@"Sounds\Explosion{i}");
                    Explosions.Add(se);
                }
            } catch (Exception e)
            {
                Console.WriteLine("Failed to initialise SoundManager. Error: {0}", e.Message);
            }
        }

        public static void PlayExplosion()
        {
            try { Explosions[Rand.Next(0, ExplosionCount)].Play(); }
            catch (Exception e) { Console.WriteLine("Failed to play sound effect. Error: {0}", e.Message); }
        }

        public static void PlayPlayerShot()
        {
            try { PlayerShot.Play(); }
            catch (Exception e) { Console.WriteLine("Failed to play sound effect. Error: {0}", e.Message); }
        }

        public static void PlayEnemyShot()
        {
            try { EnemyShot.Play(); }
            catch (Exception e) { Console.WriteLine("Failed to play sound effect. Error: {0}", e.Message); }
        }
    }
}
