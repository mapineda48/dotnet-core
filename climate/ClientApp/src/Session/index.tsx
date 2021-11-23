import React from "react";
import { initAction } from "mp48-react/useState";
import CircularProgress from "@mui/material/CircularProgress";
import Box from "@mui/material/Box";
import { prepare } from "../api";

import style from "./index.module.scss";

import type { FetchLocation } from "../api";

const useState = initAction({
  loading(state: State, loading = true): State {
    return { ...state, loading };
  },
  message(state: State, message: string): State {
    return { ...state, message };
  },
  fetch(state: State, fetch: FetchLocation): State {
    return { ...state, fetch };
  },
});

const Context = React.createContext<FetchLocation>(null as any);

export function useSession() {
  const fetch = React.useContext(Context);

  return fetch;
}

export default function History(props: Props) {
  const [state, , history] = useState({
    loading: false,
    message: "",
    fetch: null,
  });

  React.useEffect(() => {
    if (state.loading || state.message || state.fetch) return;

    history.loading();

    prepare()
      .then((api) => {
        history.fetch(api);
      })
      .catch((err) => {
        console.log(err);
        history.message(err.message || "unknown");
      })
      .finally(() => {
        history.loading(false);
      });
  }, [state, history]);

  if (state.loading) {
    return (
      <Box className="full-screen-center">
        <CircularProgress />
      </Box>
    );
  }

  if (state.message) {
    return (
      <Box className="full-screen-center">
        <p className={style.notify}>
          <strong>{state.message}</strong>
        </p>
      </Box>
    );
  }

  if (!state.fetch) return null;

  return (
    <Context.Provider value={state.fetch}>{props.children}</Context.Provider>
  );
}

/**
 * Types
 */
interface State {
  loading: boolean;
  message: string;
  fetch: FetchLocation | null;
}

interface Props {
  children: JSX.Element;
}
