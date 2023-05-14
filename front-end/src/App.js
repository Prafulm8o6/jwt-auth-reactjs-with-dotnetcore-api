import './App.css';
import { Routes, Route, useNavigate } from 'react-router-dom'
import Login from './Login';
import Register from './Register';
import { useEffect, useState } from 'react';
import axios from 'axios';
import Navbar from './Navbar';

function App() {
  const [wetherforcast, setWetherforcast] = useState([]);
  const [error, setErr] = useState();
  const navigate = useNavigate();

  const getWetherforcastData = async () => {
    var bearerToken = localStorage.getItem('token');
    let config = {
      headers: {
        'Authorization': `Bearer ${bearerToken}`
      }
    }
    axios.get(`https://localhost:44353/WeatherForecast/Get`,
      config
    ).then((response) => {
      setWetherforcast(response.data);
    }).catch((err) => {
      setErr(err.message)
      console.log(err)
    });
    console.log(wetherforcast);
  }

  useEffect(() => {
    const userInfo = sessionStorage.getItem('userInfo');
    if (userInfo === null) {
      navigate('/login', { replace: true })
    }
    getWetherforcastData()
    var bearerToken = localStorage.getItem('token');
    console.log('Bearer ', bearerToken)
  }, []);

  return (
    <>
      <div className="App">
        <header className="App-header">
          <Navbar />
          <Routes>
            <Route path='/' element={
              <>
                <h1>
                  {process.env['REACT_APP_TITLE']}
                </h1>
                {
                  (wetherforcast.length === 0 && error !== null)
                    ?
                    <h1>{error}</h1>
                    :
                    <div>
                      <pre>
                        Wetherforcast API DATA
                      </pre>
                      <table border={5}>
                        <tr>
                          <th>Date</th>
                          <th>C</th>
                          <th>F</th>
                          <th>Summary</th>
                        </tr>
                        {wetherforcast.map(e => {
                          return (
                            <tr>
                              <td>{e.date}</td>
                              <td>{e.temperatureC}</td>
                              <td>{e.temperatureF}</td>
                              <td>{e.summary}</td>
                            </tr>
                          )
                        })}
                      </table>
                    </div>
                }
              </>
            }></Route>
            <Route path='/login' element={<Login />} />
            <Route path='/register' element={<Register />} />
          </Routes>
        </header>
      </div>
    </>
  );
}

export default App;
