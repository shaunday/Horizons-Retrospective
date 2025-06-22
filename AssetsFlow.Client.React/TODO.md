## 1
defect: buttons state screwed after trade element remove 
add: IsAnyContentMissing to element on collapsed
defect : summary is  missing (on adding trade)
from limor: title should be styled diff than the content, text aligned to the left


## 2.
add: filter by : status first, date first, show: idea, interim, closed
add: filtering -... in client? / adjust data call api
add: trade command bar - delete trade action, collapse all trades 

add: expand/collapse by clicking on the background
issue: how to edit lessons in trade closure?
add: change-history modal/tooltip
add: Once an element is active, can restamp via popup (pick data/now) 
add: context/jotai for user, saved sectors, parse via useHandleStatusUpdates, sectors combobox +  autofill

## 3.
1 main notifications bar? for progress
pagination ui (parse X data)
free flow string with AI to turn it into a trade object
from limor: edit > expand the object and edit in place?
obfuscate amounts/costs for anon view - a method (in the user context?)
sector thoughts
rq: refetch inverval, cache time, background fetching notificaion, pagination ?

## Testing:
 -[] RQ: https://tanstack.com/query/latest/docs/framework/react/guides/testing
        https://tkdodo.eu/blog/testing-react-query
 -[] log/console writes ups
 -[] error management - sentry? 