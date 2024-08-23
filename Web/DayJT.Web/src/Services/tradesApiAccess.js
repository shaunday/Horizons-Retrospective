import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_SUFFIX
const tradesUrl = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_TRADES_SUFFIX;
const componentsUrl = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_COMPONENETS_SUFFIX;

const componentsClient = axios.create(baseURL + componentsUrl);
const tradesClient = axios.create(baseURL + tradesUrl);


 export async function getTradeById(clientId) {
     return await tradesClient.get(clientId)
 }

 export async function addTradeComposite() {
    return await tradesClient.get()
}

export function getSummaryElement(tradeId) {

  }

export async function updateEntry(componentId, newContent, changeNote) {
    const newContentAndInfo = {
        content: newContent,
        info: changeNote
    }

    axios.put(componentsUrl + componentId, newContentAndInfo)
    .then(response => {
        //todo
    })
    
}

