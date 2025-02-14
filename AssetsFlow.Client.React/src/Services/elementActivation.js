export async function canActivateElement(element) {
    const isValid = !element[Constants.TRADE_ENTRIES_STRING]
        .some(e => e.IsMustHave && (!e.Content || e.Content.trim() === ""));
    return isValid;
}
