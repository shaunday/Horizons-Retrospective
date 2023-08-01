import axios from "axios";

const baseURL = import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_SUFFIX
const client = axios.create(import.meta.env.VITE_API_URL + import.meta.env.VITE_JOURNAL_SUFFIX);

export function getAllTrades() {
    return client.get()
};

export function getAllTrades() {
    return client.post()
}

export function UpdateComponent(tradeInputId, componentId, newContent, changeNote) {
    return client.put({
        tradeInputId: tradeInputId,
        componentId: "This is an updated post."
    })
}

