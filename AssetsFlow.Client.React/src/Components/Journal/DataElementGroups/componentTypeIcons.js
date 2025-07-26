import { TbMenu4, TbBoxAlignTopRightFilled, 
        TbCoins, TbTarget, TbReportAnalytics, TbAdjustmentsExclamation, TbChecklist } 
  from 'react-icons/tb';

const componentTypeIcons = {
  Header: TbMenu4,
  Context: TbBoxAlignTopRightFilled,
  EntryLogic: TbChecklist,
  ExitLogic: TbTarget,
  PriceRelated: TbCoins,
  Risk: TbAdjustmentsExclamation,
  Results: TbReportAnalytics,
};

export const getComponentTypeIcon = (componentType) => {
  return componentTypeIcons[componentType] || TbMenu4;
}