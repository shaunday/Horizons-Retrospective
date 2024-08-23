import { useState } from 'react';
import * as Constants from '@constants/Constants';
import TradesApiAccess from '@services/TradesApiAccess';
import { useQuery, useMutation, useQueryClient } from 'react-query';

function useTrades() {
    const queryClient = useQueryClient();
    const [tradeIdCounter, setTradeIdCounter] = useState(1); // Start with ID 1

    // Fetch trade by ID
    const fetchTradeById = async (id) => {
        const response = await TradesApiAccess.getTradeById(id);
        return response.data;
    };
    
    // Fetch the trade corresponding to the current incremental ID
    const { data: trades } = useQuery({
        queryKey: [Constants.TRADES_KEY, tradeIdCounter],
        queryFn: async () => await fetchTradeById(tradeIdCounter),
        onSuccess: (data) => {
            const updatedData = {
                ...data,
                clientId: tradeIdCounter,
            };
            
            // Update the cache with the new data
            queryClient.setQueryData([Constants.TRADES_KEY, tradeIdCounter], updatedData);
    
            // Increment the ID for the next fetch
            setTradeIdCounter((prevId) => prevId + 1);
        },
        keepPreviousData: true  
        });

    // Mutation to add a new trade
    const addTradeMutation = useMutation(
        async () => await TradesApiAccess.addTradeComposite(), 
        {
            onSuccess: (data) => {
                // Update the cache with the new trade
                queryClient.setQueryData([Constants.TRADES_KEY, tradeIdCounter], (oldTrades = []) => {
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