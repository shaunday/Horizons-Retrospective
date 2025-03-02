export const CONTENT_VALUE_STRING = 'contentValue';
export const CONTENT_WRAPPER_STRING = 'contentWrapper';
export const CREATED_AT_STRING = 'createdAt'; 
export const CHANGE_NOTE_STRING = 'changeNote'; 

export const dataParser = (dataEle) => {
    return {
        contentValue: dataEle?.[CONTENT_WRAPPER_STRING]?.[CONTENT_VALUE_STRING] ?? "",
        createdAt: dataEle?.[CONTENT_WRAPPER_STRING]?.[CREATED_AT_STRING] ?? "",
        changeNote: dataEle?.[CONTENT_WRAPPER_STRING]?.[CHANGE_NOTE_STRING] ?? "",
    };
};