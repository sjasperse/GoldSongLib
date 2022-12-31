import axios from "axios";
import { Song } from "../types";

const apiClient = axios.create();
apiClient.interceptors.request.use(config => {

  const token = localStorage.getItem('token');
  if (token) {
    config.headers ??= {};
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

export async function getSongs(): Promise<Array<Song>> {
  const response = await apiClient('/api/songs');
  const songs = response.data;

  return songs;
}

export async function addSong(song: Song): Promise<void> {
  const response = await apiClient('/api/songs', {
    method: 'POST',
    headers: {
      'content-type': 'application/json'
    },
    data: song
  });

  return response.data;
}

export async function deleteSong(id: string): Promise<void> {
  await apiClient(`/api/songs/${id}`, {
    method: 'DELETE'
  });
}

export async function loginWithGoogleToken(googleToken: string): Promise<{ token: string}> {
  const response = await apiClient('/api/user/login', {
    method: 'POST',
    headers: {
      'content-type': 'application/json'
    },
    data: {
      googleToken
    }
  });

  return response.data;
}