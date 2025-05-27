a drawer with explanations: 
component type groups, red line for missing
origin > Idea. Add a trade and fill it > Open. Pending while element is being filled up

fix styling clutter
from limor: mark currently expanded element..
from limor: add trade - different color, more visible possibly upthere on the header area
from limor: title should be styled diff than the content, text aligned to the left
from limor - icons instead of text "+" etc - check zoom homepage?
defect: add trade doesnt refresh the added trade.

add opened/closedat somehow (to overview?) 
opened = first activate date, close = closed date, remove props from trade make em getters

## 2.
change-history modal/tooltip
Once an element is active, can restamp via popup (pick data/now) 
add context/jotai for user, saved sectors, parse via useHandleStatusUpdates, sectors combobox +  autofill
filter by : status first, date first, show: idea, interim, closed
filtering -... in client? / adjust data call api

## 3.
from limor: edit > expand the object and edit in place?
obfuscate amounts/costs for anon view - a method (in the user context?)
sector thoughts
rq: refetch inverval, cache time, background fetching notificaion, pagination ?

## Testing:
 -[] RQ: https://tanstack.com/query/latest/docs/framework/react/guides/testing
        https://tkdodo.eu/blog/testing-react-query
 -[] log/console writes ups
 -[] error management - sentry? 