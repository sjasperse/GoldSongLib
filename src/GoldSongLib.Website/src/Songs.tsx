import { lazy, useEffect, useState } from "react"
import { Link } from "react-router-dom";
import Loading from "./Loading";
import newGuid from "./services/newGuid";

type Song = {
  id: string,
  name: string
}

export default function Songs() {
  const [songs, setSongs] = useState<Song[]>();

  useEffect(() => {


    loadSongs();
  }, []);

  async function loadSongs() {
    const response = await fetch('/api/songs');
    const songs = await response.json();
    setSongs(songs);
  }

  async function deleteSong(id: string) {
    await fetch(`/api/songs/${id}`, {
      method: 'DELETE'
    });
    await loadSongs();
  }


  return <div>
    <h1>Songs</h1>
    {songs == undefined
      ? <Loading />
      : <ul>
        {songs.map(song => <li key={song.id}>{song.name} <button onClick={() => deleteSong(song.id)}>X</button></li>)}
        <li><Link to="/songs/add">Add song...</Link></li>
      </ul>
    }
  </div>
}