using Modding;
using System;
using System.Linq;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using SFCore.Utils;
using HKMirror;

namespace RandomSpells
{
    public class RandomSpellsMod : Mod
    {
        private static RandomSpellsMod? _instance;

        internal static RandomSpellsMod Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"An instance of {nameof(RandomSpellsMod)} was never constructed");
                }
                return _instance;
            }
        }

        public override string GetVersion() => GetType().Assembly.GetName().Version.ToString();

        public RandomSpellsMod() : base("RandomSpells")
        {
            _instance = this;
        }

        public override void Initialize()
        {
            Log("Initializing");

            On.PlayMakerFSM.OnEnable += SpellFsmChanges;

            Log("Initialized");
        }

        private void SpellFsmChanges(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {

            orig(self);

            if (self.gameObject.name == "Knight" && self.FsmName == "Spell Control")
            {
                self.GetFsmAction<ListenForUp>("QC", 2).Enabled = false;
                self.GetFsmAction<ListenForDown>("QC", 3).Enabled = false;
                self.GetFsmAction<BoolTest>("Spell Choice", 0).Enabled = false;
                self.GetFsmAction<BoolTest>("Spell Choice", 1).Enabled = false;

                self.AddMethod("QC", () =>
                {
                    List<string> spells = new();
                    if (PlayerDataAccess.fireballLevel > 0)
                    {
                        spells.Add("FIREBALL");
                    }
                    if (PlayerDataAccess.quakeLevel > 0)
                    {
                        spells.Add("QUAKE");
                    }
                    if (PlayerDataAccess.screamLevel > 0)
                    {
                        spells.Add("SCREAM");
                    }
                    if (spells.Any())
                    {
                        var randomSpell = UnityEngine.Random.Range(0, spells.Count);
                        self.SendEvent($"{spells[randomSpell]}");
                    }
                    else
                    {
                        self.SendEvent("FIREBALL");
                    }
                });

                self.AddMethod("Spell Choice", () =>
                {
                    List<string> spells = new();
                    if (PlayerDataAccess.fireballLevel > 0)
                    {
                        spells.Add("FIREBALL");
                    }
                    if (PlayerDataAccess.quakeLevel > 0)
                    {
                        spells.Add("QUAKE");
                    }
                    if (PlayerDataAccess.screamLevel > 0)
                    {
                        spells.Add("SCREAM");
                    }
                    if (spells.Any())
                    {
                        var randomSpell = UnityEngine.Random.Range(0, spells.Count);
                        self.SendEvent($"{spells[randomSpell]}");
                    }
                    else
                    {
                        self.SendEvent("FIREBALL");
                    }
                });
            }
        }
    }
}
