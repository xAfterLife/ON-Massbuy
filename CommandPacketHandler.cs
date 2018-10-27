const int BUY_LIMIT = 12000;
const int STACK_LIMIT = 255;

/// <summary>
/// $Buy Packet
/// </summary>
/// <param name="BuyPacket"></param>
public void Buy(OpenNos.GameObject.CommandPackets.BuyPacket buypacket)
{
    try //Not needed but safe is safe
    {
        if (buypacket != null)
        {
            if (buypacket.Amount <= BUY_LIMIT)
            {
                if (buypacket.Item != null && buypacket.Amount != 0)
                {

                    int Leftover = buypacket.Amount % STACK_LIMIT;
                    int FulLStacks = buypacket.Amount / STACK_LIMIT;
                    short BuyVNum = 0;

                    switch (buypacket.Item.ToUpper())
                    {
                        case "CELLA":
                            BuyVNum = 1014;
                            break;
                        case "FULLI":
                            BuyVNum = 1244;
                            break;
                        default:
                            return;
                    }

                    Item iteminfo = ServerManager.Instance.GetItem(BuyVNum);
                    if (Session.Character.Gold >= buypacket.Amount * iteminfo.Price)
                    {
                        for (int i = 1; i <= FulLStacks; i++)
                        {
                            ItemInstance inv = Session.Character.Inventory.AddNewToInventory(BuyVNum, STACK_LIMIT).FirstOrDefault();
                            if (inv == null)
                            {
                                Session.SendPacket(UserInterfaceHelper.Instance.GenerateMsg(Language.Instance.GetMessageFromKey("NOT_ENOUGH_PLACE"), 0));
                            }
                            else
                            {
                                Session.SendPacket(Session.Character.GenerateSay($"{Language.Instance.GetMessageFromKey("ITEM_ACQUIRED")}: {iteminfo.Name} x {STACK_LIMIT}", 12));
                                Session.Character.Gold -= STACK_LIMIT * inv.Item.Price;
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
        }
    }
    catch
    {
        //Put Errorlog here
        return;
    }

}
