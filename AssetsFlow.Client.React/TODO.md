## 1
add: seedernpm /admin panel - Use the user’s roles (from the JWT or user object) to show/hide 
fix: close trade not working
add: user login
adjust : risk group's icon, context's icon
add: update user's saved sectors (parse via useHandleStatusUpdates?), sectors combobox +  autofill
add: userinfo on toppanel, tooltip + constants

from limor: edit > expand the object and edit in place?
from limor: title should be styled diff than the content, text aligned to the left
add: opened/closedat (just to overview?)
add: verify auth is checked on gateway+ add min time on ea


## 2.
RQ isloading/error vs statusnots
add: filter (status first, date first, BY STATUS), in client? / adjust data call api
revert trades order? last added first?

 <p>Something went wrong:</p> > unify with error nott.
ThemeIcon variant="light" color="indigo.4"> + move the icon into the paper - on elementbadge  ?

add: ignoreActivation flag to content edit
add: Once an element is active, can edit date via popup (pick date/now) 
add: trade command bar - delete trade action, collapse all trades 
add: expand/collapse by clicking on the background
issue: how to edit lessons in trade closure?
add: content-history

## 3.
obfuscate amounts/costs for anon view - a method (in the user context?)
1 main notifications bar? for progress
pagination ui (parse X data)
free flow string with AI to turn it into a trade object
sector thoughts
rq: refetch inverval, cache time, background fetching notificaion, pagination ?

## Testing:
 -[] RQ: https://tanstack.com/query/latest/docs/framework/react/guides/testing
        https://tkdodo.eu/blog/testing-react-query
 -[] log/console writes ups
 -[] error management - sentry? 