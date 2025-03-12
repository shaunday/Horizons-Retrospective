import { TbMenu4, TbClockHeart, TbBrain, TbSettingsQuestion, 
        TbCoins, TbTarget, TbReportAnalytics, TbAdjustmentsStar, TbChecklist } 
  from 'react-icons/tb';

const componentTypeIcons = {
  Header: TbMenu4,
  Emotions: TbClockHeart,
  Thoughts: TbBrain,
  Technicals: TbSettingsQuestion,
  EntryLogic: TbChecklist,
  ExitLogic: TbTarget,
  PriceRelated: TbCoins,
  Risk: TbAdjustmentsStar,
  Results: TbReportAnalytics,
};

export const getComponentTypeIcon = (componentType) => {
  return componentTypeIcons[componentType] || TbMenu4;
}