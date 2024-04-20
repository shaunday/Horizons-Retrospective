import './App.css'
import JournalContainer from './Journal/Trades/Components/JournalContainer'

function App() {

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
