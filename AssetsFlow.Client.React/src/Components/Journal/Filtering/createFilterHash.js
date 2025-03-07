import { createLabeledFieldInfoStruct} from "@components/LabeledField"; 

export function createFilterHash() {
  return {
    tickers: createLabeledFieldInfoStruct("tickers", "text", "Tickers"),
    sectors: createLabeledFieldInfoStruct("sectors", "text", "Sectors"),

    openStartDate: createLabeledFieldInfoStruct("openStartDate", "date", "Creation Start Date"),
    openEndDate: createLabeledFieldInfoStruct("openEndDate", "date", "Creation End Date"),
    openStartDate: createLabeledFieldInfoStruct("closeStartDate", "date", "Closure Start Date"),
    openEndDate: createLabeledFieldInfoStruct("closeEndDate", "date", "Closure End Date"),


  };
}
