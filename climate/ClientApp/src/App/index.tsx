import React from "react";
import Grid from "@mui/material/Grid";
import Paper from "@mui/material/Paper";
import { initAction } from "mp48-react/useState";
import Box from "@mui/material/Box";
import { useSession } from "../Session";
import Root from "./Root";
import Bar from "./Bar";

import type { ResLocation } from "../api";

const useState = initAction({
  showLocation(state: State, location: string): State {
    if (state.current === location) return state;

    return { ...state, current: location };
  },
  addLocation(state: State, location: string, res: ResLocation): State {
    return {
      ...state,
      current: location,
      historys: state.historys.includes(location)
        ? state.historys
        : [...state.historys, location],
      cache: { ...state.cache, [location]: { ...res } },
    };
  },
  message(state: State, message: string): State {
    return { ...state, message };
  },
  loading(state: State, loading = true): State {
    return { ...state, loading };
  },
});

function isPrimitive(val: any) {
  return (
    typeof val === "string" ||
    typeof val === "number" ||
    typeof val === "boolean" ||
    val === undefined ||
    val === null
  );
}

function setTitle(val: string) {
  return (
    val.substr(0, 1).toLocaleUpperCase() +
    val.substr(1, val.length).replaceAll("_", " ")
  );
}

function PrintResult(props: { children: any }): JSX.Element {
  if (isPrimitive(props.children)) {
    return (
      <Grid item>
        {props.children || "unknow"}
      </Grid>
    );
  }

  return Object.entries(props.children).map(([key, val], index) => {
    const title = setTitle(key);

    if (isPrimitive(val)) {
      if (key === "url") {
        return (
          <Grid key={index} item>
            <a href={val as string} target="_blank" rel="noopener noreferrer">
              Visitar
            </a>
          </Grid>
        );
      }

      if (key === "urlToImage") {
        return (
          <Grid key={index} item>
            <img
              style={{ maxWidth: "100%" }}
              src={val as string}
              alt="Unknown"
            />
          </Grid>
        );
      }

      return (
        <Grid key={index} item>
          <p>
            <strong>{title + ": "}</strong> {val}
          </p>
        </Grid>
      );
    }

    const Pipe = Array.isArray(val) ? (
      val.map((val, index) => <PrintResult key={index}>{val}</PrintResult>)
    ) : (
      <PrintResult>{val}</PrintResult>
    );

    return (
      <React.Fragment key={index}>
        <Grid item xs={12}>
          <h2 style={{ textAlign: "center" }}>{title}</h2>
        </Grid>
        {Pipe},
      </React.Fragment>
    );
  }) as any;
}

export default function App() {
  const session = useSession();

  const [state, , dashboard] = useState({
    loading: false,
    message: "",
    historys: [...session.locations],
    current: "",
    cache: {},
  });

  const data = state.cache[state.current];

  const existsData = Boolean(data);

  const showWelcome = !existsData && !state.loading && !state.message;

  const showData = existsData && !state.loading && !state.message;

  return (
    <Root>
      <Bar
        disabled={state.loading}
        historys={state.historys}
        onSearch={(location) => {
          if (state.loading) return;

          if (state.cache[location]) {
            dashboard.showLocation(location);
            return;
          }

          dashboard.loading();

          session
            .fetchLocation(location)
            .then((res) => dashboard.addLocation(location, res))
            .catch((err) => {
              console.error(err);
              dashboard.message(err.message || "unknown");
            })
            .finally(() => dashboard.loading(false));
        }}
      />
      {showWelcome && (
        <Box className="full-screen-center">
          <p className="welcome">
            Because, yes, while the world can seem unbearable at times, being
            informed about what is happening makes us better people and
            professionals. Being informed allows you to become part of something
            bigger and connect with the rest of the world.
          </p>
        </Box>
      )}
      {showData && (
        <Grid
          ref={(ref) => {
            /**
             * Forgive me this
             */

            if (!ref) return;

            const bar = ref.previousElementSibling as HTMLDivElement;

            ref.style.marginTop = bar.offsetHeight + 50 + "px";
          }}
          padding={2}
          container
          spacing={{ xs: 2, md: 3 }}
          columns={{ xs: 4, sm: 8, md: 12 }}
          justifyContent="center"
        >
          <Grid item xs={3} sm={4} md={5}>
            <Paper>
              <Grid container justifyContent="center" padding={1} spacing={1}>
                <Grid item xs={12}>
                  <h1 style={{ textAlign: "center" }}>News</h1>
                  <hr />
                </Grid>
                <PrintResult>{data.news}</PrintResult>
              </Grid>
            </Paper>
          </Grid>
          <Grid item xs={3} sm={4} md={5}>
            <Paper>
              <Grid container justifyContent="center" padding={1} spacing={1}>
                <Grid item xs={12}>
                  <h1 style={{ textAlign: "center" }}>Weather</h1>
                  <hr />
                </Grid>
                <PrintResult>{data.weather}</PrintResult>
              </Grid>
            </Paper>
          </Grid>
        </Grid>
      )}
      {state.message && (
        <Box className="full-screen-center">{state.message}</Box>
      )}
      {state.loading && <Box className="full-screen-center">Loading...</Box>}
    </Root>
  );
}

/**
 * Types
 */
export interface State {
  loading: boolean;
  message: string;
  current: string;
  historys: string[];
  cache: {
    [K: string]: ResLocation;
  };
}
