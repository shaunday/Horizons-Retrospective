## 1

filtering: selected filter - save as jotai's atom, pass to api
selecting filter should refresh all trades

from limor: title should be styled diff than the content, text aligned to the left
add: opened/closedat/IdeaDate (just to overview?)

add: expand/collapse by clicking on the background, reduce expand icon's size
fix: edit lessons in trade closure
refresh userdata on reseed

## 2.

lint : "react/prop-types": "off"... keep fixing
try catch on parsers? why dont i get onSuccess with fetchTrades, userData
verify no triple calls

RQ isloading/error vs statusnots.. vs mantines notifications
add: sort/sort order (status first, date first)
revert trades order? last added first?

add: ignoreActivation flag to content edit (requests "DenyActivation")
add: Once an element is active, can edit date via popup (pick date/now)
add: trade command bar - delete trade action, collapse all trades

add: broker to filtering
add: content-history
add: user login ... routing?
add: userinfo on toppanel, tooltip + constants
add: data attributes/alt/role for testing

## 3.

add/parse saved sectors - free flow combobox
on reduce position, check max allowed from analytics
validation per net ammount on close - if net =0 just close

obfuscate amounts/costs for anon view - a method (in the user context?)
pagination ui (parse X data)
free flow string with AI to turn it into a trade object
sector thoughts
rq: refetch inverval, cache time, background fetching notificaion, pagination ?

## Testing:

-[] RQ: https://tanstack.com/query/latest/docs/framework/react/guides/testing
https://tkdodo.eu/blog/testing-react-query
-[] log/console writes ups
-[] error management - sentry?
