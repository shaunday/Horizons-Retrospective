import { useQuery } from '@tanstack/react-query';
import * as Constants from '@constants/constants';
import { getSummaryElement } from '@services/TradesApiAccess';

export function useTradeSummary(tradeComposite) {
    const summaryQueryKey = [Constants.TRADECOMPOSITE_SUMMARY_KEY, tradeComposite.Id];

    const { data: tradeSummary } = useQuery({
        queryKey: summaryQueryKey,
        queryFn: async () => await getSummaryElement(tradeComposite.Id),
        initialData: tradeComposite.Summary,
        refetchOnWindowFocus: false,
    });

    return tradeSummary;
}