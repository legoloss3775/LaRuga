using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace LaRuga.Enemies;

public class LaRugaEnemy : ModNPC
{
    public bool IsFeastTime;

    private float _speedMultiplier = 1.5f;

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[2];
    }

    public override void SetDefaults()
    {
        Player[] player = Main.player;
        foreach (Player player2 in player)
        {
            if (Utils.Distance(player2.position, NPC.position) <= 10f)
            {
                int defense = player2.statDefense;
                if (defense <= 0)
                {
                    defense = 1;
                }
                NPC.damage = player2.statLifeMax2 + defense;
            }
        }

        Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/larugaembient");
        NPC.knockBackResist = 0f;
        NPC.aiStyle = 0;
        NPC.boss = true;
        AnimationType = 1;
        NPC.noTileCollide = false;
        NPC.width = 160;
        NPC.height = 150;
        NPC.lifeMax = 110000;
        NPC.defense = 0;
        NPC.value = 500f;
        NPC.HitSound = SoundID.DeerclopsScream;
        NPC.DeathSound = SoundID.DeerclopsDeath;
    }

    public override void AI()
    {
        if (NPC.justHit)
        {
            StartFeast();
        }
        if (!IsFeastTime)
        {
            return;
        }
        NPC.TargetClosest(true);
        Vector2 position = Main.player[NPC.target].position;
        if (position.X < NPC.position.X && NPC.velocity.X > -12f * _speedMultiplier)
        {
            NPC.velocity.X -= 0.22f * _speedMultiplier;
        }
        if (position.X > NPC.position.X && NPC.velocity.X < 12f * _speedMultiplier)
        {
            NPC.velocity.X += 0.22f * _speedMultiplier;
        }
        if (position.Y < NPC.position.Y)
        {
            if (NPC.velocity.Y < 0f && NPC.velocity.Y > -6f * _speedMultiplier)
            {
                NPC.velocity.Y -= 0.7f * _speedMultiplier;
            }
            else
            {
                NPC.velocity.Y -= 0.8f * _speedMultiplier;
            }
        }
        if (position.Y > NPC.position.Y)
        {
            if (NPC.velocity.Y > 0f && NPC.velocity.Y < 6f * _speedMultiplier)
            {
                NPC.velocity.Y += 0.7f * _speedMultiplier;
            }
            else
            {
                NPC.velocity.Y += 0.8f * _speedMultiplier;
            }
        }
        NPC nPC = NPC;
        nPC.position = nPC.position + NPC.velocity;
    }

    private void StartFeast()
    {
        IsFeastTime = true;
        NPC.noTileCollide = true;
        Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/larugafight");
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        // 3284 - ColossalSquid
        return ((ModNPC)ModLoader.GetMod("CalamityMod").GetContent().ToArray()[3284]).SpawnChance(spawnInfo);
    }

    public override void FindFrame(int frameHeight)
    {
        if (NPC.velocity.Y == 0f)
        {
            NPC.spriteDirection = NPC.direction;
            if (NPC.velocity.X == 0f)
            {
                NPC.frame.Y = 0;
                NPC.frameCounter = 0.0;
            }
            NPC nPC = NPC;
            nPC.frameCounter += 1.0;
            if (NPC.frameCounter > 5.0)
            {
                NPC.frame.Y = NPC.frame.Y + 158;
                NPC.frameCounter = 0.0;
            }
            if (NPC.frame.Y >= 158)
            {
                NPC.frame.Y = 0;
            }
        }
        else
        {
            if (NPC.velocity.Y < -12f * _speedMultiplier)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = 158;
            }
            if (NPC.velocity.Y > 12f * _speedMultiplier)
            {
                NPC.frameCounter = 0.0;
                NPC.frame.Y = 158;
            }
        }
    }
}
