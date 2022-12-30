import { Song } from "../types";

export async function getSongs(): Promise<Array<Song>> {
  const response = await fetch('/api/songs');
  const songs = await response.json();

  return songs;
}

export async function deleteSong(id: string): Promise<void> {
  const response = await fetch(`/api/songs/${id}`, {
    method: 'DELETE'
  });
}