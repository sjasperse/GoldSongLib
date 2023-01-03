import { lazy, useContext, useEffect, useState } from "react"
import { Link } from "react-router-dom";
import { AppContext } from "./AppContext";
import Loading from "./Loading";
import { getSongs } from "./services/apiClient";
import newGuid from "./services/newGuid";

type Song = {
  id: string,
  name: string
}

export default function Songs() {
  const { songs, refreshSongs } = useContext(AppContext);

  async function deleteSong(id: string) {
    await deleteSong(id);
    await refreshSongs();
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