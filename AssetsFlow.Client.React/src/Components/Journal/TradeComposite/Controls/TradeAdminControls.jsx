import React from "react";

function TradeAdminControls({ trade }) {

 return (
     <>
       {/* <div>
         <Menu shadow="md" width={150} position="bottom-end">
           <Menu.Target>
             <ActionIcon variant="subtle">
               <TbDots />
             </ActionIcon>
           </Menu.Target>
           <Menu.Dropdown>
             <Menu.Item leftSection={<TbTrash size={16} />}
               onClick={() => handleAction(ElementActions.DELETE)}
             >
               Delete Element
             </Menu.Item>
           </Menu.Dropdown>
         </Menu>
       </div>
       <ProcessingAndSuccessMessage status={processingStatus} /> */}
     </>
   );
 }
 
 export default React.memo(TradeAdminControls);