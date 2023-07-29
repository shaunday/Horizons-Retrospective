import React from 'react';

function Cell({cell, updateCellContent}) {
    return (
        <div id="cell">
            <div id="cellTitle">{cell.Title} </div>
            <div id="cellContent">{cell.ContentWrapper.Content} </div>
        </div> )};

export default Cell;