import { IconMenu4, IconClockHeart, IconBrain, IconSettingsQuestion, 
  IconCoins, IconTarget, IconReportAnalytics, IconAdjustmentsStar, IconChecklist } from '@tabler/icons-react';

const componentTypeIcons = {
  Header: IconMenu4,
  Emotions: IconClockHeart,
  Thoughts: IconBrain,
  Technicals: IconSettingsQuestion,
  EntryLogic: IconChecklist,
  ExitLogic: IconTarget,
  PriceRelated: IconCoins,
  Risk: IconAdjustmentsStar,
  Results: IconReportAnalytics,
};

export const getComponentTypeIcon = (componentType) => {
  return componentTypeIcons[componentType] || IconMenu4;
}