import './App.css'
import JournalContainer from './Journal/Trades/Components/JournalContainer'

function App() {

  const [allTrades, setAllTrades] = React.useState(null);
 setAllTrades(TradesApiAccess.getAllTrades()); //dont use hooks to fetch data => data is fetched after render 
 //todo derive values in render

 //alternatively const { data } = useQuery... // which one to use??

  return (
    <div id="vwrapper">
      <div id="header">Header placeholder</div>
      <div id="mainBody">
        <div className="flexChildCenter gotRightSideNeighbour">Metrics placeholder</div>
        <JournalContainer/>
      </div>
      <div id="footer">Footer placeholder</div>
    </div>
  )
}

export default App
