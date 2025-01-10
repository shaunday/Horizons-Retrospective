import { createLabeledFieldInfoStruct} from "@components/LabeledField"; 

export function createFilterHash() {
  return {
    ticker: createLabeledFieldInfoStruct("tickers", "text", "Tickers"),
    startDate: createLabeledFieldInfoStruct("startDate", "date", "Start Date"),
    endDate: createLabeledFieldInfoStruct("endDate", "date", "End Date"),
  };
}
