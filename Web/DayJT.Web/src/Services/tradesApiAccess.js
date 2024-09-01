import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_SUFFIX
const tradesUrl = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_TRADES_SUFFIX;
const componentsUrl = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_COMPONENETS_SUFFIX;

const tradesClient = axios.create(baseURL + tradesUrl);

export async function getAllTrades() {
    const response = await tradesClient.get();
    return response.data;
}

 export async function getTradeById(clientId) {
    const response = await tradesClient.post(clientId);
    return response.data;
}

 export async function addTradeComposite() {
    return await tradesClient.get()
}

export async function addReduceInterimPosition(tradeId, isAdd) {
    const response = await axios.post(tradesUrl + tradeId, isAdd);
    return response.data;
}

export async function closeTrade(tradeId, closingPrice) {
    const response = await axios.post(tradesUrl + tradeId + "/close", closingPrice);
    return response.data;
}

export async function updateEntry(componentId, newContent, changeNote) {
    const newContentAndInfo = {
        content: newContent,
        info: changeNote
    }
    const response = await axios.put(componentsUrl + componentId, newContentAndInfo);
    return response.data;
    }
    

