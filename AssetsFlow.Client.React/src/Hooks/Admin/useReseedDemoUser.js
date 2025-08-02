// src/hooks/Admin/useReseedDemoUser.js
import { useState } from "react";
import { notifications } from "@mantine/notifications";
import { reseedDemoUserAPI } from "@services/ApiRequests/adminApiAccess";
import { tradeKeysFactory } from "@services/query-key-factory";
import { useQueryClient } from "@tanstack/react-query";

export function useReseedDemoUser() {
  const [isReseeding, setIsReseeding] = useState(false);
  const queryClient = useQueryClient();

  const reseed = async () => {
    setIsReseeding(true);
    try {
      await reseedDemoUserAPI();
      queryClient.invalidateQueries({ queryKey: tradeKeysFactory.getKeyForAllTrades() });
      queryClient.invalidateQueries({ queryKey: tradeKeysFactory.getKeyForAllTradesByIds() });
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