import React from 'react';
import { IngredientList } from './features/Ingredients';
import { RecipeList } from './features/Recipes';
import { Login, useAuthUser } from './features/Auth';
import './custom.css';
import 'react-toastify/dist/ReactToastify.min.css';
import { PrivateLayout, PublicLayout } from './components/Layouts';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Home } from './features/Home';

function App() {
	const { isLoggedIn } = useAuthUser();

	return (
		<div className="flex flex-col w-screen h-screen overflow-hidden antialiased">
			<BrowserRouter>
				<Routes>
					{/* private layout with private route children */}
					{isLoggedIn ? (
						<Route element={<PrivateLayout />}>
							<Route path="/" element={<Home />} />							 
							{/* route marker - remove if you don't want feature routes added by default */}
							<Route path="/ingredients" element={<IngredientList />} />
							<Route path="/recipes" element={<RecipeList />} />
						</Route>
					) : null}

					{/* public layout with public route children */}
					<Route element={<PublicLayout />}>
						{/* // TODO do this on server side */}
						{!isLoggedIn ? <Route path="/" element={<Login />} /> : null}
						<Route path="/login" element={<Login />} />
					</Route>
				</Routes>
			</BrowserRouter>
		</div>
	);
}

export default App;
