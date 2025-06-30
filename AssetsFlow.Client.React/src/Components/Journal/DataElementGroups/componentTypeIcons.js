import { TbMenu4, TbLayoutList, 
        TbCoins, TbTarget, TbReportAnalytics, TbAdjustmentsStar, TbChecklist } 
  from 'react-icons/tb';

const componentTypeIcons = {
  Header: TbMenu4,
  Context: TbLayoutList,
  EntryLogic: TbChecklist,
  ExitLogic: TbTarget,
  PriceRelated: TbCoins,
  Risk: TbAdjustmentsStar,
  Results: TbReportAnalytics,
};

export const getComponentTypeIcon = (componentType) => {
  return componentTypeIcons[componentType] || TbMenu4;
}