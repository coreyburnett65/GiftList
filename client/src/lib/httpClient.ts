import Axios, { type AxiosError, type AxiosRequestConfig } from 'axios';

// withCredentials is what makes the browser send the ASP.NET Core Identity auth
// cookie on every request; without it, the cookie is silently never sent and
// "login doesn't stick" is the confusing symptom you'd see later.
export const axiosInstance = Axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL ?? '/',
  withCredentials: true,
});

// orval generates one call to this function per API operation instead of using
// axios directly, so every generated hook shares this baseURL/credentials setup.
export const customInstance = <T>(config: AxiosRequestConfig): Promise<T> => {
  const source = Axios.CancelToken.source();

  const promise = axiosInstance({ ...config, cancelToken: source.token }).then(
    ({ data }) => data as T,
  );

  // @ts-expect-error -- orval's react-query client calls .cancel() on abort
  promise.cancel = () => source.cancel('Query was cancelled');

  return promise;
};

export type ErrorType<TError> = AxiosError<TError>;
