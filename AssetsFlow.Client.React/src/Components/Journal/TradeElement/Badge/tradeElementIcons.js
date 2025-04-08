import { TbBulb, TbCirclesRelation, TbCirclePlus, TbCircleMinus, TbSum } from "react-icons/tb";

const tradeElementIcons = {
  Origin: TbBulb,
  Add: TbCirclePlus,
  Reduce: TbCircleMinus,
  Evaluation: TbCirclesRelation,
  Summary: TbSum,
};

export const getTradeElementStatusIcon = (status) => {
  return tradeElementIcons[status] || TbBulb;
};