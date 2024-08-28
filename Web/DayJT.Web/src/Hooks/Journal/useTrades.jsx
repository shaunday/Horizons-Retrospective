import { useState } from 'react';
import * as Constants from '@constants/constants';
import { getTradeById, addTradeComposite} from '@services/tradesApiAccess';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

function useTrades() {
    const queryClient = useQueryClient();
    const [tradeIdCounter, setTradeIdCounter] = useState(1); // Start with ID 1

    // Fetch trade by ID
    const fetchTradeById = async (id) => {
        const response = await getTradeById(id);
        return response.data;
    };
    
    // Fetch the trade corresponding to the current incremental ID
    const { data: trades } = useQuery({
        queryKey: [Constants.TRADE_KEY, tradeIdCounter],
        queryFn: async () => await fetchTradeById(tradeIdCounter),
        onSuccess: (data) => {
            const updatedData = {
                ...data,
                [Constants.TRADE_CLIENTID_PROP_STRING]: tradeIdCounter,
            };
            
            // Update the cache with the new data
            queryClient.setQueryData([Constants.TRADE_KEY, tradeIdCounter], updatedData);
    
            // Increment the ID for the next fetch
            setTradeIdCounter((prevId) => prevId + 1);
        },
        keepPreviousData: true  
        });

    // Mutation to add a new trade
    const addTradeMutation = useMutation(
        async () => await addTradeComposite(), 
        {
            onSuccess: (data) => {
                // Update the cache with the new trade
                queryClient.setQueryData([Constants.TRADE_KEY, tradeIdCounter], (oldTrades = []) => {
                    return [...oldTrades, data]; // Append the new trade to the existing trades
                });
                setTradeIdCounter((prevId) => prevId + 1);
            },
            onError: (error) => {
                console.error('Error adding trade:', error);
            }
        }
    );

    return {
        trades,       // List of trades from the cache
        addTrade: addTradeMutation.mutate, // Function to add a new trade
    };
}