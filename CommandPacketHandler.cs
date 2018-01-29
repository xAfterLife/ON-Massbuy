/// <summary>
/// $Buy Packet
/// </summary>
/// <param name="BuyPacket"></param>
public void Buy(OpenNos.GameObject.CommandPackets.BuyPacket buypacket)
{
    if (buypacket != null)
    {

        int Leftover = buypacket.Amount % 255;
        int FulLStacks = buypacket.Amount / 255;
        short BuyVNum = 0;

        switch (buypacket.Item.ToUpper())
        {
            case "RGB":
                BuyVNum = 1429;
                break;
            case "CELLA":
                BuyVNum = 1014;
                break;
        }

        Item iteminfo = ServerManager.Instance.GetItem(BuyVNum);
        if (Session.Character.Gold >= buypacket.Amount * iteminfo.Price)
        {
            for (int i = 1; i <= FulLStacks; i++)
            {
                ItemInstance inv = Session.Character.Inventory.AddNewToInventory(BuyVNum, 255).FirstOrDefault();
                if (inv == null)
                {
                    Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                }
                else
                {
                    Session.SendPacket(Session.Character.GenerateSay($"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {iteminfo.Name} x {255}", 12));
                    Session.Character.Gold -= 255 * inv.Item.Price;
                }
            }

            if (Leftover > 0)
            {
                ItemInstance inv = Session.Character.Inventory.AddNewToInventory(BuyVNum, (byte)Leftover).FirstOrDefault();
                if (inv == null)
                {
                    Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                }
                else
                {
                    Session.SendPacket(Session.Character.GenerateSay($"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {iteminfo.Name} x {Leftover}", 12));
                    Session.Character.Gold -= Leftover * inv.Item.Price;
                }
            }
        }
        else
        {
            Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_MONEY"), 0));
        }
        Session.SendPacket(Session.Character.GenerateGold());
    }

}
