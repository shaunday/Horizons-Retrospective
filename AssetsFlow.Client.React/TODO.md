## 1
from limor: title should be styled diff than the content, text aligned to the left
defect: add trade doesnt refresh the added trade.
defect: missing doesnt update on add trade
defect: buttons state screwed after trade element remove 
defect: edit button skips when clicked
add: click on trade element to expand/collapse
issue: not all dataelements appear on overview, if they've got missing content it wont be visible

## 2.
add: delete trade action (when trade is expanded)
add: change-history modal/tooltip
add: Once an element is active, can restamp via popup (pick data/now) 
add: context/jotai for user, saved sectors, parse via useHandleStatusUpdates, sectors combobox +  autofill
add: filter by : status first, date first, show: idea, interim, closed
add: filtering -... in client? / adjust data call api

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