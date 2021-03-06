using System;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
    public class TeleportPetTarget : Target
    {
        private TeleportPetDeed m_Deed;

        public TeleportPetTarget(TeleportPetDeed deed)
            : base(1, false, TargetFlags.None)
        {
            m_Deed = deed;
        }

        protected override void OnTarget(Mobile from, object target)
        {
            if (m_Deed.Deleted || m_Deed.RootParent != from)
                return;

            if (target is LifeStone)
            {
                LifeStone stone = target as LifeStone;

                if (stone.RootParent != from)
                {
                    from.SendMessage("You must be carrying the Life Stone.");
                }
                if (stone.TeleportPets)
                {
                    from.SendMessage("That stone already teleports pets.");
                }
                else
                {
                    stone.TeleportPets = true;
                    from.SendMessage("That stone will now teleport your pets!");

                    m_Deed.Delete();
                }
            }
            else
            {
                from.SendMessage("That isn't a Life Stone!");
            }
        }
    }

    public class TeleportPetDeed : Item
    {
        public override string DefaultName
        {
            get { return "teleport pet deed for a life stone"; }
        }

        [Constructable]
        public TeleportPetDeed()
            : base(0x14F0)
        {
            Weight = 1.0;
            Hue = 1196;
            LootType = LootType.Blessed;
        }

        public TeleportPetDeed(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // Version;
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001);
            }
            else
            {
                from.SendMessage("Target the Life Stone you wish to make teleport pets.");
                from.Target = new TeleportPetTarget(this);
            }
        }
    }
}