import { useState } from "react";
import { notifications } from "@mantine/notifications";
import { reseedDemoUserAPI } from "@services/ApiRequests/adminApiAccess";
import { useClearTradesCache } from "@hooks/UserManagement/useClearTradesCache";

export function useReseedDemoUser() {
  const [isReseeding, setIsReseeding] = useState(false);
  const clearTradesCache = useClearTradesCache();

  const reseed = async () => {
    setIsReseeding(true);
    try {
      await reseedDemoUserAPI();
      clearTradesCache();

      notifications.show({
        title: "Success",
        message: "Demo user and trades reseeded!",
        color: "green",
      });
    } catch (error) {
      notifications.show({
        title: "Error",
        message: "Failed to reseed demo user. Please try again.",
        color: "red",
      });
    } finally {
      setIsReseeding(false);
    }
  };

  return { isReseeding, reseed };
}