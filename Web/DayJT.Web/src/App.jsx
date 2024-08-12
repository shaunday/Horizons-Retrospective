import './App.css'
import * as TradesApiAccess from './../Services/TradesApiAccess'
import * as Constants from './constants/constants'
import {useAllTrades} from './Hooks/useAllTrades'
import {
  useQuery,
  QueryClient,
  QueryClientProvider
} from '@tanstack/react-query'
import JournalContainer from './Journal/Trades/Components/JournalContainer'

function App() {

 const queryClient = new QueryClient()
 const allTradesQuery = useAllTrades()


if (allTradesQuery.status === 'pending') {
  return <span>Loading...</span>
}

if (allTradesQuery.status === 'error') {
  return <span>Error: {allTradesQuery.error.message}</span>
}

  return (
    <QueryClientProvider client={queryClient}>
      <div id="vwrapper">
        <div id="header">Header placeholder</div>
        <div id="mainBody">
          <div className="flexChildCenter gotRightSideNeighbour">Metrics placeholder</div>
          <JournalContainer trades={allTradesQuery.data}/>
        </div>
        <div id="footer">Footer placeholder</div>
      </div>
    </QueryClientProvider>
  )
}

export default App
