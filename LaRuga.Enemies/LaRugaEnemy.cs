using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LaRuga.Enemies;

public class LaRugaEnemy : ModNPC
{
    public bool IsFeastTime;

    private void StartFeast()
    {
        IsFeastTime = true;
        NPC.noTileCollide = true;
        NPC.noGravity = true;
        Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/larugafight");
    }

    private Vector2 GetPlayerDirection()
    {
        if (!NPC.HasValidTarget) return Vector2.Zero;
        Vector2 player_center = Main.player[NPC.target].Center;
        return Vector2.Normalize(NPC.Center.DirectionTo(player_center));
    }

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[2];
    }

    public override void SetDefaults()
    {
        Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/larugaembient");
        NPC.knockBackResist = 1f;
        NPC.aiStyle = NPCAIStyleID.FaceClosestPlayer;
        NPC.boss = true;
        AnimationType = NPCID.None;
        NPC.noTileCollide = false;
        NPC.width = 206;
        NPC.height = 158;
        NPC.lifeMax = 110067;
        NPC.defense = 1;
        NPC.damage = 9999;
        NPC.value = 500f;
        NPC.HitSound = SoundID.DeerclopsScream;
        NPC.DeathSound = SoundID.DeerclopsDeath;
        NPC.netAlways = true;
        NPC.noGravity = false;
    }

    public override void AI()
    {
        if (!IsFeastTime)
        {
            if (NPC.justHit)
            {
                StartFeast();
            }
        }
        else
        {
            NPC.velocity = Vector2.Lerp(NPC.velocity, 40f * GetPlayerDirection(), .05f);
            NPC.rotation = NPC.direction * MathF.Sin(.01f * NPC.velocity.Length());
        }
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        if (ModLoader.TryGetMod("CalamityMod", out Mod calamity_mod)) {
            if (calamity_mod.TryFind("ColossalSquid", out ModNPC colossal_squid)) {
                return colossal_squid.SpawnChance(spawnInfo);
            }
        }
        return 0; // colossal squid does not exist
    }

    public override void FindFrame(int frameHeight)
    {
        if (!IsFeastTime)
        {
            NPC.frame.Y = 0;
            return;
        }

        NPC.frameCounter += Math.Round(NPC.velocity.Length());
        NPC.spriteDirection = NPC.direction;
        if (NPC.frameCounter >= 100f) {
            NPC.frame.Y = NPC.frame.Y >= NPC.height ? 0 : frameHeight;
            NPC.frameCounter = 0;
        }
    }
}
