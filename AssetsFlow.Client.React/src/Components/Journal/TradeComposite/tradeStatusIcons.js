import { TbBulb, TbProgress, TbCheckupList } from "react-icons/tb";

const tradeStatusIcons = {
  AnIdea: TbBulb,
  Open: TbProgress,
  Closed: TbCheckupList,
};

export const getTradeStatusIcon = (status) => {
  return tradeStatusIcons[status] || TbBulb;
};