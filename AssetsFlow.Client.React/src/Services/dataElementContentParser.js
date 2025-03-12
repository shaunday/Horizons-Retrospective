const CONTENT_VALUE_STRING = 'contentValue';
const CONTENT_WRAPPER_STRING = 'contentWrapper';
const CREATED_AT_STRING = 'createdAt'; 
const CHANGE_NOTE_STRING = 'changeNote'; 
const DATA_RESTRICTION_STRING = 'restrictions'

export const dataElementContentParser = (dataEle) => {
    return {
        contentValue: dataEle?.[CONTENT_WRAPPER_STRING]?.[CONTENT_VALUE_STRING] ?? "",
        createdAt: dataEle?.[CONTENT_WRAPPER_STRING]?.[CREATED_AT_STRING] ?? "",
        changeNote: dataEle?.[CONTENT_WRAPPER_STRING]?.[CHANGE_NOTE_STRING] ?? "",
        textRestrictions: dataEle?.[DATA_RESTRICTION_STRING] ?? {} ,
    };
};