import axios from "axios";

const CURRENT_HISTORY = "Client History Id";

const historyId = localStorage.getItem(CURRENT_HISTORY);

export async function prepare(baseUrl = "/") {
  const url = baseUrl + "api/history/" + (historyId || "-1");

  const { data } = await axios.get<ResHistory>(url);

  const id = data.id.toString();

  if (!historyId) {
    localStorage.setItem(CURRENT_HISTORY, id.toString());
  }

  return {
    locations: data.location?.map((val) => val.name) || ([] as string[]),
    async fetchLocation(name: string) {
      const url = baseUrl + `api/external/?location=${name}`;

      const { data } = await axios.get(url, { headers: { "History-ID": id } });

      return data as ResLocation;
    },
  };
}

/**
 * Types
 */
export type FetchLocation<T = ReturnType<typeof prepare>> = T extends Promise<
  infer R
>
  ? R
  : never;

export interface ResLocation {
  weather: Weather;
  news: News;
}

export interface Weather {
  coord: {
    lon: number;
    lat: number;
  };
  weather: {
    id: number;
    main: string;
    description: string;
    icon: string;
  }[];
  base: string;
  main: {
    temp: number;
    feels_like: number;
    temp_min: number;
    temp_max: number;
    pressure: number;
    humidity: number;
  };
  visibility: number;
  wind: {
    speed: number;
    deg: number;
  };
  clouds: {
    all: number;
  };
  dt: number;
  sys: {
    type: number;
    id: number;
    country: string;
    sunrise: number;
    sunset: number;
  };
  timezone: number;
  id: number;
  name: string;
  cod: number;
}

export interface News {
  status: string;
  totalResults: number;
  articles: Partial<Article>[];
}

export interface Article {
  source: {
    id: string;
    name: string;
  };
  author: string;
  title: string;
  description: string;
  url: string;
  urlToImage: string;
  publishedAt: string;
  content: string;
}

export interface ResHistory {
  id: number;
  location: null | { id: number; name: string }[];
}
