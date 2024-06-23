import './App.css'
import * as TradesApiAccess from './../Services/TradesApiAccess'
import * as Constants from './constants/constants'
import {
  useQuery,
  QueryClient,
  QueryClientProvider
} from '@tanstack/react-query'
import JournalContainer from './Journal/Trades/Components/JournalContainer'

function App() {

  const [allTrades, setAllTrades] = React.useState(null);
 setAllTrades(TradesApiAccess.getAllTrades()); //dont use hooks to fetch data => data is fetched after render 
 //todo derive values in render

 const queryClient = new QueryClient()
 const { status, data, error } = useQuery({
  queryKey: [Constants.ALL_TRADES_KEY],
  queryFn: async () => await TradesApiAccess.getAllTrades().data
})

if (status === 'pending') {
  return <span>Loading...</span>
}

if (status === 'error') {
  return <span>Error: {error.message}</span>
}

  return (
    <QueryClientProvider client={queryClient}>
      <div id="vwrapper">
        <div id="header">Header placeholder</div>
        <div id="mainBody">
          <div className="flexChildCenter gotRightSideNeighbour">Metrics placeholder</div>
          <JournalContainer/>
        </div>
        <div id="footer">Footer placeholder</div>
      </div>
    </QueryClientProvider>
  )
}

export default App
